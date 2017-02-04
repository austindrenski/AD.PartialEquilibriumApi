# AD.PartialEquilibriumApi
C# framework for programming Partial Equilibrium models.

## Install from NuGet:
```Powershell
PM> Install-Package AD.PartialEquilibriumApi
```

## Purpose
This library aims to provide the tools necessary to construct partial equilibrium models that are object-oriented, simple to create, and extensible.

Implementations of this library are known as Extensible Partial Equilibrium Modeling (ExPEM) type models. An official implementation of this library is currently under development and can be previewed under: [AD.PartialEquilibriumApi.Example](https://github.com/austindrenski/AD.PartialEquilibriumApi/tree/master/AD.PartialEquilibriumApi.Example). 

## Motivation
Partial equilibrium (PE) models are simple and lightweight, but programming one can be complicated by the hierarchical nature of the linkages. This library proposes the use of the Extensible Markup Language (XML) to represent the model in an intuitive and object-oriented manner.

XML is designed to represent relational data. This works well for PE models characterized by layers of horizontal and vertical linkages. However, object-oriented design is not a principle concern of the XML specification, and PE models are easily imagined as the interaction between objects (e.g. markets, products). To align these two characteristics, this library relies on the .NET Language-Integrated Query (LINQ) library which provides for the object-oriented navigation of complicated relational linkages.

The primary goal of this library is to help write code that mirrors the graphical representation of a model.

```
                                                -----------------
                                                |  Downstream   | 
                                                -----------------
                                               /                 \   
                                              /                   \   
                                             /                     \         
                            -----------------                       -----------------
                            | Upstream 1    |                       | Upstream 2    |
                            -----------------                       -----------------
                           /                 \                                       \
                          /                   \                                       \
                         /                     \                                       \
        -----------------                       -----------------                       -----------------    
        | Upstream 3    |                       | Upstream 4    |                       | Upstream 5    |
        -----------------                       -----------------                       -----------------
```
```XML
<Downstream>
  <Upstream1 />
    <Upstream3 />
    <Upstream4 />
  <Upstream2 />
    <Upstream5 />
</Downstream>
```
```C#
XElement downstream = new XElement(...);
double sumOfSquaresOfChildren = downstream.Select(x => x * x).Sum();
```
