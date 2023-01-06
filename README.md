# EditorConfig Guidelines

EditorConfig Guidelines extension add support for new `guideline` property
in [`.editorconfig`](https://editorconfig.org/). This property adds column
guideline in text editor:

![preview-1.png](art/preview-1.png)


Sample `.editorconfig`:
```
root = true

[*]
guidelines = 80, 100
```

### Features
- .editorconfig zero-configuration. Ideal for teams.
- Theming support.
- Customizable guideline color.
- **No** telemetry.

### Changelog

* **1.1.0** (06 January 2023)
  * Add option to specify guidelines style (solid, dashed, dotted)
  * Suggest to rate extension on Visual Studio Marketplace.

* **1.0.3** (26 December 2022)
  * Update icon.

* **1.0.1** (26 December 2022)
  * Update display name.

* **1.0.0** (26 December 2022)
  * Initial release.
