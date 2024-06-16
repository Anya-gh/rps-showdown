import { Routes, Route } from "react-router-dom"
import Home from "./home/Home"
import About from "./About/About"
import Play from "./Play/Play"
import Stats from "./Stats/Stats"
import { useState } from "react"

function App() {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  return (
    <>
      <Routes>
        <Route path="" element={<Home username={username} setUsername={setUsername} password={password} setPassword={setPassword}/>} />
        <Route path="/about" element={<About />} />
        <Route path="/play" element={<Play />} />
        <Route path="/stats" element={<Stats />} />
      </Routes>
    </>
  )
}

// make requests type safe

export default App
