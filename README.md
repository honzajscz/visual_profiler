 # Visual Profiler 
This repository contains code and its detailed explanation made for 
[Master Thesis **Performance Profiling for .NET Platform**](https://dspace.cvut.cz/handle/10467/8738)
Czech Technical University, Faculty of Electrical Engineering 
Prague, Czech Republic
2011 - 2012


  
## Abstract
The performance profiling is a powerful way how to get a valuable insight into software applications. In order to be precise and efective, a performance profiler needs to run with low performance and memory overhead and present the result in a transparent way to a developer. In this thesis we analyze performance profiling in the context of Microsoft .NET platform, with focus on the C# programming language, and introduce our own implementation of a profiler, the Visual Profiler. The Visual Profiler features tracing and sampling profiling engines together with an innovative way of presenting the profiling results within the integrated development environment Microsoft Visual Studio 2010.

  

## Profiler in Action (YouTube)

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/_IRkdeGGtJo/0.jpg)](https://www.youtube.com/watch?v=_IRkdeGGtJo)

  

## Detailed Description and Presentation
Here is a link to the master thesis in pdf
- [Master thesis in pdf in English](Thesis/thesis.pdf)
- [Final presentation in PDF in Czech](Thesis/Presentation/Výkonové%20profilování%20na%20platformě.pdf)
- [Final presentation in PPTX in Czech](Thesis/Presentation/Výkonové%20profilování%20na%20platformě.pptx)

## Used Technologies and Frameworks
Assembler, C++, COM, Profiling API, Win32 API, Named pipes, .NET, C#, Ninject, Linq, NUnit, Moq, WPF, XAML, Visual Studio 2010 Extension API, VSIX packages, MEF…

## Repository Content
- The **Install directory** contains the VSIX package for deploying the Visual Proler extension into Visual Studio 2010 and a C# file for enabling proling on assemblies.
- The **Source directory** contains the source code of this thesis, its dependencies and the Mandelbrot testing application.
There are three solutions:
  - **VisualProfiler.sln** - the main solution, contains VisualProfilerBackend, VisualProfilerAccess, VisualProlerUI
  - **Mandlerbrot\Mandelbrot.sln** - the testing application, contains Mandelbrot and MandelbrotComp
projects
  - **Ccimetadata\Metadata.sln** - the Common Compiler Infrastructure Metadata API, contains projects for reading PDB files

- The **Thesis directory** contains the PDF document of this thesis.