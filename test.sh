# Run from top-level directory
npm install ./frontend
dotnet restore ./backend
dotnet restore ./backend_tests
npm run test --prefix ./frontend && cd ./backend_tests && dotnet test && cd ..