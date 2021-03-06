This repository contains a set of useful controls for the Universal Windows Platform (UWP) written in C#.

## Controls

The following controls are currently hosted in this repository:

- [ArrayPanel](docs/ArrayPanel.md)
- [ScrollSelector](docs/ScrollSelector.md)
- [TimeSpanEditor](docs/TimeSpanEditor.md)
- [TimeSpanPicker](docs/TimeSpanPicker.md)

For details, please see the documentation page for each control.

## Features

The following features apply to all the controls in this repository:

- **Theme compliant** - all controls use theme resources for styling and are tested on both light and dark themes.
- **Fully templatable** (where applicable)
- **Globalized** - all controls use culture-aware formatting of dates and numbers, and support RTL layouts.
- **Accessible** - all controls support both mouse, keyboard and touch interactions.

## Getting started

We are currently in the process of adding CI/CD to enable automatic build and binary download creation, as well as automatic publishing of the controls as a public NuGet package.

For now you need to use the controls as source code. You have a few options:

- Use git submodules/subtree to incorporate this repository into your own
- Copy/paste parts or all of the code in this repository into your own solution

## SDK compatibility

- Target SDK version: Windows 10 1809 build 17764
- Minimum SDK version: Windows 10 1803 build 17134

## Contributions

We welcome contributions of all kinds. Bug fixes and minor improvements are greatly appreciated, as well as new features. Please create issues and pull requests.

## Licensing

The code in this repository is released under the MIT license.