# Visualiser Drawing Functionality - Implementation Summary

## Overview
The Visualiser component has been extended to support canvas-based drawing in addition to text and markup output. This allows Advent of Code solutions to create visual representations using drawing commands.

## Files Created

### 1. `Web/Components/AoC/DrawingCommand.cs`
Defines the drawing command types and structure:
- `DrawingCommandType` enum: Canvas, Colour, Move, Line, Point
- `DrawingCommand` record: Holds command data (type, coordinates, color)

### 2. `Web/Components/AoC/VisualiserFrame.cs`
Represents a single frame in the visualiser history:
- `VisualiserFrame` record: Contains text output, drawing commands, and canvas dimensions
- `HasDrawing` property: Indicates if the frame contains drawing commands

### 3. `Web/Components/AoC/VisualiserDrawingCommands.md`
Comprehensive documentation for using the drawing functionality:
- Command syntax reference
- Usage examples
- Animation techniques
- Best practices

### 4. `Solutions/2025/VisualiserDrawingExample.cs`
Example implementation demonstrating various drawing capabilities:
- Simple shapes (box)
- Complex patterns (spiral)
- Grid-based visualization (pathfinding)
- Animation (wave pattern)

## Files Modified

### 1. `Web/Components/AoC/Visualiser.razor`
**Major Changes:**
- Updated to use `VisualiserFrame` instead of string-based history
- Added canvas state tracking (_canvasWidth, _canvasHeight, _currentX, _currentY, _currentColour)
- Enhanced `VisualiseOutput()` to parse drawing commands
- Added SVG rendering for drawing mode
- Maintained backward compatibility with text/markup output

**New Features:**
- Dual rendering mode (text vs. canvas)
- SVG-based drawing with scalable graphics
- Text overlay support when combining drawing with text
- Frame-based history with full command replay

### 2. `Web/Components/AoC/Visualiser.razor.css`
**New CSS Classes:**
- `.canvas-mode`: Styling for canvas display mode
- `.drawing-canvas`: SVG canvas styling with proper sizing
- `.text-overlay`: Semi-transparent text display over drawings

## Drawing Commands Supported

### Canvas Command
```
canvas:width:height
```
Initializes or resizes the virtual canvas.

### Colour Command
```
colour:colorValue
color:colorValue
```
Sets the current drawing color. Supports named colors, hex codes, RGB, and HSL.

### Move Command
```
move:x:y
```
Moves the pen position without drawing.

### Line Command
```
line:x1:y1:x2:y2
```
Draws a line from (x1, y1) to (x2, y2).

### Point Command
```
point:x:y
```
Draws a point at (x, y).

## Usage Example

```csharp
// In your solution class
private static Action<string[], bool>? _visualise = null;

[Init]
public static void Init(string[] input, Action<string[], bool>? visualise = null)
{
    _visualise = visualise;
}

public static int Part1()
{
    // Initialize canvas
    _visualise?.(["canvas:800:600"], true);
    
    // Draw something
    _visualise?.([
        "colour:red",
        "line:100:100:700:500",
        "point:400:300"
    ], false);
    
    // Your solution code...
    return result;
}
```

## Key Features

1. **Backward Compatible**: Existing text and markup visualizations continue to work unchanged
2. **SVG-Based**: Scalable graphics that work at any resolution
3. **Frame History**: All drawing states are preserved and can be replayed
4. **Animation Support**: Multiple frames can be created for animated visualizations
5. **Color Support**: Named colors, hex, RGB, and HSL color formats
6. **Mixed Mode**: Can combine text output with drawings in the same frame

## Technical Details

- Uses SVG rendering for maximum compatibility and scalability
- Coordinate system: (0, 0) at top-left corner
- Drawing commands are cumulative within a frame
- Canvas state (position, color) persists between calls unless cleared
- Timeline controls work seamlessly with both text and drawing modes

## Testing

All code compiles successfully and follows the project's .editorconfig style guidelines.
The implementation has been tested with the existing build system.

## Next Steps

To use the drawing functionality in your solutions:
1. Add `Action<string[], bool>? visualise` parameter to your Init/Solution methods
2. Call `visualise` with drawing command strings
3. Use the examples in `VisualiserDrawingExample.cs` as reference
4. Refer to `VisualiserDrawingCommands.md` for complete command documentation
