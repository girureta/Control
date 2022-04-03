# Control
- [Control](#control)
- [Overview](#overview)
- [Features](#features)
- [Usage](#usage)
  - [Setup](#setup)
  - [Manual run](#manual-run)
  - [Automated run](#automated-run)
- [Customizing / Extending.](#customizing--extending)
- [Dependencies:](#dependencies)
- [Future experiments](#future-experiments)

# Overview

A test automation driver for Unity applications.

![ControlScreenshotAppiumInspector](docs/screenshot.png)

Control implements WebDriver commands for interacting with Unity applications. This enables us to test how the application runs on specifc platforms and environments using already exiting WebDriver/Appium client libraries.

The package is intented to be extensible and allow developers to 
expose new elements or customize how existing ones are presented and driven.

# Features

Currently some parts of **Scene/GameObject/Component** and **UIToolkit**  is exposed, enough to allow the application to be inspected and driven using [Appium Inspector](https://github.com/appium/appium-inspector).


| Feature | Description  | Windows | Android | 
|---|---|---|---|
| GetSource | Returns the current state of the application in XML| :heavy_check_mark: | :question: |
| Click | Click on an element| :heavy_check_mark: | :question: |
| SendKeys | Send keys to an element| :heavy_check_mark: | :question: |
| Clear | Cleat the text of an element| :heavy_check_mark: | :question: |
| TakeScreenshot | Take a screenshot of the whole application | :heavy_check_mark: | :question: |





# Usage

## Setup
* Place **ControlBehaviour** in a Scene that is loaded and shipped with your application. 

> **URL:** Take note of the Url in the behaviour, change it at will.

* Run the application!. 
 
> The driver is part of the application so we must run it before using the driver.

## Manual run

The application may be run manually both in the Editor and as a build, after which we can run our tests towards it or connect with an inspector.


## Automated run

In automated scenarions like CI, you can use Appium Hub with Appium client libraries to install/start/uninstall the Unity application in Windows/Android/iOS for you.

When the application is running you can start the tests that point towards the **Control** driver's URl and target the actual content.

!!!
Here we need an example


# Customizing / Extending.

New or custom elements can be created by inheriting from **ElementDriver**.

SourceObjectType being the type of the project that this class is driving and from whose instance this class is created from.

In this new implementation we can override **PopulateSource** to customize how an element is represented as a XML node, or **Click** to decide how it should handle a click.



All implementations must by registered manually in the factory. DriverBehaviour register the elements implemented by default,  but the registered elements can ve customized by overriding ?????.

Examples:

- [GameObject Element](package/Runtime/Elements/General/GameObjectElement.cs)

- [Transform Element](package/Runtime/Elements/General/TransformElement.cs)

# Dependencies:

Embedio: A MIT licensed web server intended for ease of use and embedding in other application. It is used for the WebDriver commands.

# Future experiments

* Use DOM manipulation in WebGL to expose information and react to commands.
* Runtime loading/injection of Control's dlls. To allow developers to created builds without **Control** and just load/inject when running tests.