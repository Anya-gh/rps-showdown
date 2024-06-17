import { Routes, Route } from "react-router-dom"
import Home from "./Home/Home"
import About from "./About/About"
import Play from "./Play/Play"
import Stats from "./Stats/Stats"
import Credits from "./Credits/Credits"

function App() {

  return (
    <>
      <Routes>
        <Route path="" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/play" element={<Play />} />
        <Route path="/stats" element={<Stats />} />
        <Route path="/credits" element={<Credits />} />
      </Routes>
    </>
  )
}

// make requests type safe

export default App
