# Visualiser Drawing Commands

The Visualiser component now supports both text/markup output and canvas-based drawing.

## Drawing Commands

All drawing commands are passed as strings in the `VisualiseOutput()` method. Commands are case-insensitive and use colon-separated values.

### Canvas Command
Initialize or resize the virtual canvas:
```
canvas:width:height
```
**Example:**
```csharp
visualiser.VisualiseOutput(["canvas:800:600"]);
```

### Colour Command
Set the current drawing color for subsequent drawing operations:
```
colour:colorValue
color:colorValue
```
**Supported color formats:**
- Named colors: `red`, `blue`, `green`, `white`, `yellow`, etc.
- Hex colors: `#FF0000`, `#00FF00`, `#0000FF`
- RGB colors: `rgb(255, 0, 0)`

**Example:**
```csharp
visualiser.VisualiseOutput(["colour:red"]);
visualiser.VisualiseOutput(["color:#00FF00"]);
```

### Move Command
Move the pen to a new position without drawing:
```
move:x:y
```
**Example:**
```csharp
visualiser.VisualiseOutput(["move:100:200"]);
```

### Line Command
Draw a line from (x1, y1) to (x2, y2):
```
line:x1:y1:x2:y2
```
The pen position is automatically updated to (x2, y2) after drawing.

**Example:**
```csharp
visualiser.VisualiseOutput(["line:10:10:100:100"]);
```

### Point Command
Draw a point at (x, y):
```
point:x:y
```
**Example:**
```csharp
visualiser.VisualiseOutput(["point:50:50"]);
```

## Usage Examples

### Basic Drawing
```csharp
// Initialize canvas
visualiser.VisualiseOutput(["canvas:400:400"]);

// Draw a red square
visualiser.VisualiseOutput([
    "colour:red",
    "line:50:50:350:50",
    "line:350:50:350:350",
    "line:350:350:50:350",
    "line:50:350:50:50"
]);

// Add some blue points
visualiser.VisualiseOutput([
    "colour:blue",
    "point:100:100",
    "point:300:100",
    "point:200:200",
    "point:100:300",
    "point:300:300"
]);
```

### Drawing with Text Overlay
You can combine drawing commands with text output:
```csharp
visualiser.VisualiseOutput([
    "canvas:600:400",
    "colour:green",
    "line:0:200:600:200",
    "Step 1: Drawing horizontal line"
]);
```

### Animation
Create animated sequences by calling `VisualiseOutput` multiple times:
```csharp
for (int i = 0; i < 360; i += 10) {
    double x = 200 + 100 * Math.Cos(i * Math.PI / 180);
    double y = 200 + 100 * Math.Sin(i * Math.PI / 180);
    
    visualiser.VisualiseOutput([
        $"colour:hsl({i}, 100%, 50%)",
        $"line:200:200:{x}:{y}"
    ]);
}
```

### Clearing the Screen
Use the `clearScreen` parameter to start fresh:
```csharp
visualiser.VisualiseOutput(["canvas:800:600"], clearScreen: true);
```

## Notes

- Canvas coordinates use the top-left corner as (0, 0)
- Drawing commands are cumulative within a frame unless `clearScreen: true` is used
- The visualiser automatically detects drawing commands and switches to canvas mode
- Text output can still be added alongside drawing commands
- All frames are preserved in history and can be replayed using the timeline controls
