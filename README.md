[![test](https://github.com/girureta/Control/actions/workflows/main.yml/badge.svg)](https://github.com/girureta/Control/actions/workflows/main.yml)

# Control
- [Control](#control)
- [Overview](#overview)
- [Features](#features)
- [Usage](#usage)
  - [Setup](#setup)
  - [Load prebuild assembly](#load-prebuild-assembly)
  - [Manual run](#manual-run)
  - [Automated run](#automated-run)
  - [Android](#android)
- [Customizing / Extending.](#customizing--extending)
- [Samples](#samples)
- [Dependencies:](#dependencies)
- [Future experiments](#future-experiments)

# Overview

A test automation driver for Unity applications.

![ControlScreenshotAppiumInspector](docs/screenshot.png)

**Control** implements WebDriver commands for interacting with Unity applications. This enables us to inspect applications and automate tests on specifc platforms and environments using already exiting WebDriver/Appium client libraries.

The package is intented to be extensible and to allow developers to 
expose new elements or customize how existing ones are presented and driven.

# Features

Currently some parts of **Scene/GameObject/Component** and **UIToolkit**  are exposed, enough to allow the application to be inspected and driven using [Appium Inspector](https://github.com/appium/appium-inspector).


| Feature | Description  | Windows | Android | 
|---|---|---|---|
| GetSource | Returns the current state of the application in XML| :heavy_check_mark: | :heavy_check_mark: |
| Click | Click on an element| :heavy_check_mark: | :heavy_check_mark: |
| SendKeys | Send keys to an element| :heavy_check_mark: | :heavy_check_mark: |
| Clear | Clear the text of an element| :heavy_check_mark: | :heavy_check_mark: |
| TakeScreenshot | Take a screenshot of the whole application | :heavy_check_mark: | :heavy_check_mark: |
| GetAttribute | Returns an element's [attribute]( docs/getelementAttribute.md) | :heavy_check_mark: | :heavy_check_mark: |




# Usage

Use this URL to add the package using the Unity Package Manager:
> https://github.com/girureta/Control.git?path=/packages/Control

## Setup
* Place **ControlBehaviour** in a Scene that is loaded and shipped with your application. 

> **URL:** Take note of the URL in the behaviour, change it at will.

* Run the application!. 
 
> The driver is part of the application so we must run it before using the driver.

## Load prebuild assembly
Control can be loaded on a project without having to include the package or the source code in the Unity project.

* For this we need to load some assemblies using a script similar to the [example](projects/LoadControlAssembly.cs) 
* And copy the following assemblies into target application's executable folder: 
  * Control.Runtime.dll
  * EmbedIO.dll
  * Swan.Lite.dll 

## Manual run

The application may be run manually both in the Editor and as a build, after which we can run our tests towards it or connect with an inspector.


## Automated run

In automated scenarions you can use Appium Hub with Appium client libraries to install/start/uninstall for you the Unity application in Windows/Android/iOS.

When the application is running you can start the tests that point towards the **Control** driver's URl and target the actual content.

## Android

To connect to an Android app running on a device on port 4723, create a [bridge](https://developer.android.com/studio/command-line/adb) to the device:
> adb forward tcp:4723 tcp:4723
 
Then point your client to:
> localhost:4723

# Customizing / Extending.

New or custom elements can be created by inheriting from **ElementDriver**. We can override **PopulateSource** to customize how an element is represented as a XML node, or **Click** to decide how it should handle a click.


Since all **ElementDriver** elements are registered manually, we must customize **ControlBehaviour**
and override  **GetWebElementFactory** to include
new/custom elements.

Examples:

- [GameObject Element](package/Runtime/Elements/General/GameObjectElement.cs)

- [Transform Element](package/Runtime/Elements/General/TransformElement.cs)

# Samples

- [Javascript and wdio](projects/SampleWdio/README.md)

# Dependencies:

[Embedio](https://github.com/unosquare/embedio): Used for  the WebDriver requests.

# Future experiments

* Use DOM manipulation in WebGL to expose information and react to commands.