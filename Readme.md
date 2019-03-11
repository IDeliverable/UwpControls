This repository contains a set of useful controls for the Universal Windows Platform (UWP).

## Controls

The following controls are currently hosted in this repository:

- [ArrayPanel](docs/ArrayPanel.md)
- [ScrollSelector](docs/ScrollSelector.md)
- [TimeSpanEditor](docs/TimeSpanEditor.md)

For details, please see the documentation page for each control.

## Roadmap

We are currently working on adding a `TimeSpanPicker` control which has the same functionality as `TimeSpanEditor` but implemented as a picker control with a flyout, much like the navite `DateTimePicker` control.

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

## Contributions

We welcome contributions of all kinds. Bug fixes and minor improvements are greatly appreciated, as well as new features. Please create issues and pull requests.

## Licensing

The code in this repository is released under the MIT license.