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
