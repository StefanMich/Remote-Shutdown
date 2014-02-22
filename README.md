Remote-Shutdown
===============

A shutdown application able to be controlled via a simple interface both locally and remotely. Android app is a long term goal

Contents:

C# project:
A server that is capable of shutting down the local machine as well as receiving messages from a client telling it to shut down.

A client capable of sending messages to a server telling it to shut down after a specified amount of time or at a set time. Also receives messages from the server if another client is shutting down the server

Note: Compiling the DEBUG build configuration will result in a version that does NOT shutdown the server when the timer reaches 0

Android project:
A client with same capabilites as the c# client (work in progress)
