using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using VisualProfilerAccess.Metadata;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccess.ProfilingData
{
    public class ProfilerCommunicator<TCallTree> where TCallTree : CallTree
    {
        private readonly ICallTreeFactory<TCallTree> _callTreeFactory;
        private readonly MetadataDeserializer _metadataDeserializer;
        private readonly MetadataCache<MethodMetadata> _methodCache;
        private readonly ProfilerTypes _profilerType;
        private readonly EventHandler<ProfilingDataUpdateEventArgs<TCallTree>> _updateCallback;

        public ProfilerCommunicator(
            ProfilerTypes profilerType,
            ICallTreeFactory<TCallTree> callTreeFactory,
            MetadataDeserializer metadataDeserializer,
            MetadataCache<MethodMetadata> methodCache,
            EventHandler<ProfilingDataUpdateEventArgs<TCallTree>> updateCallback)
        {
            Contract.Requires(callTreeFactory != null);
            Contract.Requires(updateCallback != null);

            _callTreeFactory = callTreeFactory;
            _metadataDeserializer = metadataDeserializer;
            _methodCache = methodCache;
            _updateCallback = updateCallback;
            _profilerType = profilerType;
        }

        public void ReceiveActionFromProfilee(Stream byteStream, out bool finishProfiling)
        {
            Actions receivedAction = byteStream.DeserializeActions();
            switch (receivedAction)
            {
                case Actions.SendingProfilingData:
                    DispatchSendingProfilingDataAction(byteStream);
                    break;

                case Actions.ProfilingFinished:
                    finishProfiling = true;
                    return;

                default:
                    goto case Actions.ProfilingFinished;
            }

            finishProfiling = false;
        }

        private void DispatchSendingProfilingDataAction(Stream byteStream)
        {
            var streamLength = byteStream.DeserializeUint32();
            var profilingDataBytes = new byte[streamLength];
            byteStream.Read(profilingDataBytes, 0, profilingDataBytes.Length);

            List<TCallTree> callTrees;
            using (var profilingDataStream = new MemoryStream(profilingDataBytes))
            {
                _metadataDeserializer.DeserializeAllMetadataAndCacheIt(profilingDataStream);

                callTrees = new List<TCallTree>();
                while (profilingDataStream.Position < profilingDataStream.Length)
                {
                    TCallTree callTree = _callTreeFactory.GetCallTree(profilingDataStream, _methodCache);
                    callTrees.Add(callTree);
                }
            }

            var eventArgs = new ProfilingDataUpdateEventArgs<TCallTree>
                                {
                                    Action = Actions.SendingProfilingData,
                                    ProfilerType = _profilerType,
                                    CallTrees = callTrees
                                };

            ThreadPool.QueueUserWorkItem(notUsed => _updateCallback(this, eventArgs));
        }

        public void SendCommandToProfilee(Stream byteStream, Commands commandToSend)
        {
            byte[] commandBytes = BitConverter.GetBytes((UInt32) commandToSend);
            byteStream.Write(commandBytes, 0, commandBytes.Length);
        }
    }
}