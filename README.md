# Dependencies
To run application in development mode you should have dotnet 7 and npm installed

npm packages:
* tsc - TypeScript compiler for extensions
* webpack and webpack-cli - For bundling extensions
* yarn - just in case


# Running

Make sure to clone all submodules
git clone --recurse-submodules https://github.com/BerezkovN/graphics-playground.git


When running for the first time, you have to manually run "npm install" in the following directories:
```
./
./wwwroot/extensions/filesystem
./wwwroot/extensions/webgl-viewer
```

Because we use TypeScript for extensions, you must also run "npm run compile-web" for each extension:
```
./wwwroot/extensions/filesystem   (Currently is broken because uses example code)
./wwwroot/extensions/webgl-viewer
```

Afterwards, go back to the main director and run "npm start" that is equivalent to:

```bash
dotnet run --project backend/Playground/
```

