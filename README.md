# AdventOfCode
My solutions aand a Blazor front end for Advent of Code at https://adventofcode.com/

![Deploy ASP.NET Core Web Application to smabAdventOfCode](https://github.com/smabuk/AdventOfCode/workflows/Deploy%20ASP.NET%20Core%20Web%20Application%20to%20smabAdventOfCode/badge.svg)

## Blazor front-end
Currently hosted at https:/smabadventofcode.azurewebsites.com currently only links to my progress, but if you host it yourself
you can see your own data by change the appsettings.json fileand providing your own session cookie.

## Solving
On each page there are several buttons available:

### Copy
Copies the value(s) to the clipboard

### Jump
This opens a page in a new tab to the puzzle page on https://adventofocode.com/ so you can enter your result

### AoC
Retrieves the logged in user's data from the Advent of Code web site https://adventofcode.com and then solves for it.

### GitHub
Select one of the known GitHub users from the dropdown and then press the button to retieve their data from GitHub (if available) and then soves for it

This currently has a bug if the data doesn't exist yet

### Use
Solves for whatever is in the Input Area

## Solutions
Next to the GitHub a solution link will appear that jumps directly to that user's solution.
