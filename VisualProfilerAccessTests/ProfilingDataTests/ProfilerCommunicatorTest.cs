using System;
using System.IO;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using VisualProfilerAccess;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccess.ProfilingData.CallTreeElems;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccessTests.ProfilingDataTests
{
    [TestFixture]
    public class ProfilerCommunicatorTest
    {
        #region Setup/Teardown

        [SetUp]
        public void TestSetUp()
        {
            _profilingDataUpdateEventArgs = null;
            _updateCallbackFinishedSync.Reset();
        }

        #endregion

        private ProfilerCommunicator<StubCallTree> _profilerCommunicator;
        private ProfilingDataUpdateEventArgs<StubCallTree> _profilingDataUpdateEventArgs;
        private readonly AutoResetEvent _updateCallbackFinishedSync = new AutoResetEvent(false);

        private readonly Mock<MetadataCache<MethodMetadata>> _mockMethodCache =
            new Mock<MetadataCache<MethodMetadata>>();

        [TestFixtureSetUp]
        public void SetUpAttribute()
        {
            var mockMetadataDeserializer = new Mock<MetadataDeserializer>(MockBehavior.Default, null, null, null, null,
                                                                          null);
            mockMetadataDeserializer.Setup(mmd => mmd.DeserializeAllMetadataAndCacheIt(It.IsAny<Stream>()));
            Mock<ICallTreeFactory<StubCallTree>> mockTreeFactory = MockTreeFactory();

            _profilerCommunicator = new ProfilerCommunicator<StubCallTree>(
                ProfilerTypes.TracingProfiler,
                mockTreeFactory.Object,
                mockMetadataDeserializer.Object,
                _mockMethodCache.Object,
                UpdateCallback);
        }

        private Mock<ICallTreeFactory<StubCallTree>> MockTreeFactory()
        {
            Mock<ICallTreeElemFactory<StubCallTreeElem>> mockTreeElemFactory = MockTreeElemFactory();
            var mockStream = new Mock<Stream>();
            var mockTree = new Mock<StubCallTree>(MockBehavior.Default, mockStream.Object, mockTreeElemFactory.Object,
                                                  _mockMethodCache.Object);
            var mockTreeFactory = new Mock<ICallTreeFactory<StubCallTree>>();
            mockTreeFactory.Setup(mtf => mtf.GetCallTree(It.IsAny<Stream>(), It.IsAny<MetadataCache<MethodMetadata>>()))
                .
                Returns(mockTree.Object).Callback<Stream, MetadataCache<MethodMetadata>>(
                    (st, met) =>
                        {
                            const int stubTreeByteCount = 1;
                            var bytes = new byte[stubTreeByteCount];
                            st.Read(bytes, 0, stubTreeByteCount);
                        });
            return mockTreeFactory;
        }

        private Mock<ICallTreeElemFactory<StubCallTreeElem>> MockTreeElemFactory()
        {
            var mockStream = new Mock<Stream>();

            var mockTreeElemFactory = new Mock<ICallTreeElemFactory<StubCallTreeElem>>();
            var mockTreeElem = new Mock<StubCallTreeElem>(MockBehavior.Default, mockStream.Object,
                                                          mockTreeElemFactory.Object, _mockMethodCache.Object);
            mockTreeElemFactory.Setup(
                tef => tef.GetCallTreeElem(It.IsAny<Stream>(), It.IsAny<MetadataCache<MethodMetadata>>())).Returns(
                    mockTreeElem.Object);
            return mockTreeElemFactory;
        }

        private void UpdateCallback(object sender,
                                    ProfilingDataUpdateEventArgs<StubCallTree> profilingDataUpdateEventArgs)
        {
            _profilingDataUpdateEventArgs = profilingDataUpdateEventArgs;
            _updateCallbackFinishedSync.Set();
        }

        [Test]
        public void ErrorActionTest()
        {
            var memoryStream = new MemoryStream();

            byte[] actionBytes = BitConverter.GetBytes((int) Actions.Error);
            memoryStream.Write(actionBytes, 0, actionBytes.Length);
            memoryStream.Position = 0;

            bool finishProfiling;
            _profilerCommunicator.ReceiveActionFromProfilee(memoryStream, out finishProfiling);

            Assert.IsTrue(finishProfiling);
            Assert.AreEqual(memoryStream.Length, memoryStream.Position);
            Assert.IsNull(_profilingDataUpdateEventArgs);
        }

        [Test]
        public void ProfilingFinishedActionTest()
        {
            var memoryStream = new MemoryStream();

            byte[] actionBytes = BitConverter.GetBytes((int) Actions.ProfilingFinished);
            memoryStream.Write(actionBytes, 0, actionBytes.Length);
            memoryStream.Position = 0;

            bool finishProfiling;
            _profilerCommunicator.ReceiveActionFromProfilee(memoryStream, out finishProfiling);

            Assert.IsTrue(finishProfiling);
            Assert.AreEqual(memoryStream.Length, memoryStream.Position);
            Assert.IsNull(_profilingDataUpdateEventArgs);
        }

        [Test]
        public void SendCommandToProfileeTest()
        {
            var stream = new MemoryStream();
            _profilerCommunicator.SendCommandToProfilee(stream, Commands.SendProfilingData);
            Assert.AreEqual(4, stream.Position);
            stream.Position = 0;
            var sentCommands = (Commands) stream.DeserializeUint32();
            Assert.AreEqual(Commands.SendProfilingData, sentCommands);
        }

        [Test]
        public void SendingProfilingDataActionTest()
        {
            var memoryStream = new MemoryStream();

            byte[] actionBytes = BitConverter.GetBytes((int) Actions.SendingProfilingData);
            memoryStream.Write(actionBytes, 0, actionBytes.Length);

            const uint numberOfCallTreesInStream = 4;
            const uint streamLength = numberOfCallTreesInStream;

            byte[] streamLengthBytes = BitConverter.GetBytes(streamLength);
            memoryStream.Write(streamLengthBytes, 0, streamLengthBytes.Length);

            byte[] metadataLenthBytes = BitConverter.GetBytes((uint) 0);
            memoryStream.Write(metadataLenthBytes, 0, metadataLenthBytes.Length);
            memoryStream.Position = 0;

            bool finishProfiling;
            _profilerCommunicator.ReceiveActionFromProfilee(memoryStream, out finishProfiling);

            Assert.IsFalse(finishProfiling);
            Assert.AreEqual(memoryStream.Length, memoryStream.Position);

            _updateCallbackFinishedSync.WaitOne();
            Assert.IsNotNull(_profilingDataUpdateEventArgs);
            Assert.IsNotNull(_profilingDataUpdateEventArgs.CallTrees);
            Assert.AreEqual(numberOfCallTreesInStream, _profilingDataUpdateEventArgs.CallTrees.Count());
            Assert.AreEqual(ProfilerTypes.TracingProfiler, _profilingDataUpdateEventArgs.ProfilerType);
            Assert.AreEqual(Actions.SendingProfilingData, _profilingDataUpdateEventArgs.Action);
        }
    }
}