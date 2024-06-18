# Run from top-level directory
npm install ./frontend
dotnet restore ./backend
dotnet restore ./backend_tests
npm run dev --prefix ./frontend & dotnet run --project ./backend