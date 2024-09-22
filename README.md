# Overview

A template, free to use to get your console applications bootstraped with unit testing included. This template provides 3 projects as part of the template.

`1)` The Application Project, this is where you write your code.
`2)` The Tests Project, this is where you write tests for your code.
`3)` The Benchmarks Projects, this is where you write benchmarks for your code.

<br/>

## Tests With Fluent Assertions

Inspect the `.csproj` files, you will notice the `XUnit` and `FluentAssertions` Libraries included in the `template-tests`.  This is to aid or encourage you to not only write good software, but to build excellent software with unit tests.

## How to install and use
Essentially, clone this repo, then install the template locally, and then use the templete for your projects.

### Step 1 - Git Clone

```bash
git clone https://github.com/Cyballistics/template-csharp-console.git
```

### Step 2 - Install/Uninstall Template

```bash
## Navigate into the cloned repo first
dotnet new --install .

## Or in newer versions of .NetCore
dotnet new install .

## If uninstalling
dotnet new uninstall .
```

### Step 3 - Use it

```bash
## Long Form
dotnet new  cyballistics-console -n YourNewProjectName

## Short Form
dotnet new cc -n YourNewProjectName
```

<br/><hr/>

# Testing
You are welcome to change the automation tool you want, we use NodeJs for simple automation, you have `package.json` file incldued in the template, and assuming you have NodeJs installed you will be able to run `npm run` commands. At the time of this writing the default template json looks like this below, and pay special attenton to the `scirpts` section.

<br/>

```bash
npm run clear:nuget:local   # cleans the nuget local app cache
npm run dotnet:tests        # Runs your tests
npm run build:all           # Builds your project and unit tests assemblies
npm run build:core          # Builds your application only, i.e., the core
npm run build:tests         # BUilds your applications tests
npm run clean:all           # Cleans local caches of your app and tests folder
npm run clean:core          # Cleans only the core
npm run clean:tests         # Cleans only the tests
```

<br/>

```diff
{
  "name": "csharp-template-helper",
  "version": "1.0.0",
  "main": "index.js",
  "type": "module",
+  "scripts": {
    "clear:nuget:local": "dotnet nuget locals all --clear",
    "dotnet:tests": "dotnet test ./tests/SharpExperiments.Tests/SharpExperiments.Tests.csproj --logger \"console;verbosity=detailed\"",
    "build:all": "npm run build:core && npm run build:tests",
    "build:core": "cd ./src/SharpExperiments && dotnet build",
    "build:tests": "cd ./tests/SharpExperiments.Tests && dotnet build",
    "clean:all": "npm run clean:core && npm run clean:tests",
    "clean:core": "cd ./src/SharpExperiments && dotnet clean && rm -rf obj/ bin/",
    "clean:tests": "cd ./tests/SharpExperiments.Tests/ && dotnet clean && rm -rf obj/ bin/",
    "dotnet:watch": "dotnet watch --project ./src/SharpExperiments/src/SharpExperiments.csproj"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "description": ""
}
```