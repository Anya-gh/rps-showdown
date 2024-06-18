import { Routes, Route } from "react-router-dom"
import Play from "./Play/Play"
import Stats from "./Stats/Stats"
import Credits from "./Credits/Credits"
import Home from "./Home/Home"

function App() {

  return (
    <>
      <Routes>
        <Route path="" element={<Home />} />
        <Route path="/play" element={<Play />} />
        <Route path="/stats" element={<Stats />} />
        <Route path="/credits" element={<Credits />} />
      </Routes>
    </>
  )
}

export default App
