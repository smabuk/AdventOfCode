# AdventOfCode
My solutions and a Blazor front end for Advent of Code at https://adventofcode.com/

![Deploy ASP.NET Core Web Application to smabAdventOfCode](https://github.com/smabuk/AdventOfCode/workflows/Deploy%20ASP.NET%20Core%20Web%20Application%20to%20smabAdventOfCode/badge.svg)

## Blazor front-end
Currently hosted at https:/smabadventofcode.azurewebsites.com currently only links to my progress, but if you host it yourself
you can see your own data by change the appsettings.json file and providing your own session cookie.

## Solving
On each page there are several buttons available:

### Copy
Copies the value(s) to the clipboard

### Jump
This opens a page in a new tab to the puzzle page on https://adventofocode.com/ so you can enter your result

### AoC
Retrieves the logged in user's data from the Advent of Code web site https://adventofcode.com 
and then solves for it.

### GitHub
Select one of the known GitHub users from the drop-down and then press the button to retrieve 
their data from GitHub (if available) and then solves for it

This currently has a bug if the data doesn't exist yet

### Select a user
Selecting a user will show a link to that users solution and change the GitHub button to point to their input data.

### Use
Tries to solve for whatever is in the Input Area, so anyone can post in their own data and get a solution
(if I've written one!).
(May crash with invalid data)

## Solutions
Next to the GitHub a solution link will appear that jumps directly to that user's solution.

## Future planned improvements
Allow for users to login by providing their own session cookie value.
This would be stored in a cookie on their own machine and never saved anywhere on the server.
