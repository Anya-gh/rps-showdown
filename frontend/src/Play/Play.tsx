import Header from "../components/Header"
import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import ValidateUser from "../components/ValidateUser"
import Modal from "react-modal"
import { PlayType } from "../Types"
import ChooseOption from "./ChooseOption"
import Spectate from "./Spectate"
import ConfirmChange from "./ConfirmChange"
import PlayerSide from "./PlayerSide"
import OpponentSide from "./OpponentSide"
import signOut from "../components/SignOut"

/*
TODO:
- navigation/sign out
*/

export default function Play() {

  const navigate = useNavigate()

  useEffect(() => {
    ValidateUser(navigate)
    Modal.setAppElement('body')
  }, [])

  const [choice, setChoice] = useState<string>()
  const [playResponse, setPlayResponse] = useState<PlayType>()
  const [wins, setWins] = useState(0)
  const [draws, setDraws] = useState(0)
  const [losses, setLosses] = useState(0)
  const [showLevels, setShowLevels] = useState(false)
  const [level, setLevel] = useState<number | undefined>()
  const [chosenLevel, setChosenLevel] = useState<number | undefined>(level)
  const [player, setPlayer] = useState<number>(-1)
  const [showPlayers, setShowPlayers] = useState(false)
  const [chosenPlayer, setChosenPlayer] = useState<number>(player)
  const [modalOpen, setModalOpen] = useState(false)
  const [error, setError] = useState<string>()
  const playerNames = ["Player", "Beginner", "Intermediate", "Advanced"]

  const handleSetPlayer = (newChosenPlayer: number) => {
    const actualChosenPlayer = newChosenPlayer == 0 ? -1 : newChosenPlayer
    setChosenPlayer(actualChosenPlayer)
    setShowPlayers(false)
    if (level == undefined) { 
      // If a level hasn't been selected yet, freely change player without creating a session.
      setPlayer(actualChosenPlayer)
    }
    else if (wins == 0 && draws == 0 && losses == 0) {
      handleStart(chosenLevel, actualChosenPlayer)
    }
    else {
      setModalOpen(true)
    }
  }

  const handleSetLevel = (newChosenLevel: number) => {
    setChosenLevel(newChosenLevel);
    setShowLevels(false);
    setError(undefined)
    if ((level == undefined) || ((wins == 0 && draws == 0 && losses == 0))) { handleStart(newChosenLevel, chosenPlayer) } 
    else { setModalOpen(true) }
  }

  const handleStart = (newLevel: (number | undefined), newPlayer: number) => {
    // Ensure chosenLevel has been set; if not do nothing
    if (newLevel != undefined) {
      setWins(0)
      setDraws(0)
      setLosses(0)
      setChoice(undefined)
      setPlayResponse(undefined)
      setLevel(newLevel)
      setChosenLevel(newLevel)
      setPlayer(newPlayer)
      setChosenPlayer(newPlayer)
      createSession(newLevel, newPlayer)
    }
    setModalOpen(false)
  }

  const handleCancel = () => {
    setChosenLevel(level)
    setModalOpen(false)
  }

  async function createSession(newLevel: (number | undefined), newPlayer: number) {
    // have a look at the better way to do this later
    console.log(localStorage.getItem("username"))
    if (newLevel == undefined) { throw new Error("newLevel undefined."); }
    const levelID = newLevel + 1
    const playerID = newPlayer == 0 ? -1 : newPlayer
    console.log(JSON.stringify({
      "Username": localStorage.getItem("username"),
      "LevelID": levelID
    }))
    try {
      const request = await fetch("http://localhost:5000/create-session", {
        method: "POST",
        headers: {
          "Content-Type" : "application/json",
          "Authorization" : `bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify({
          "Username": localStorage.getItem("username"),
          "LevelID": levelID,
          "PlayerID" : playerID
        })
      })
      if (request.ok) {
        const response = await request.json() as number
        localStorage.setItem("session", response.toString())
        console.log(response)
      }
      else {
        console.error(request.statusText);
      }
    }

    catch (e: unknown) {
      // If there's an error reset level
      setLevel(undefined)
      if (typeof e === "string") {
        e.toUpperCase()
      } 
      else if (e instanceof Error) {
        e.message 
      }
    }
  }

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <ConfirmChange modalOpen={modalOpen} setModalOpen={setModalOpen} chosenLevel={chosenLevel} chosenPlayer={chosenPlayer} handleCancel={handleCancel} handleStart={handleStart}/>

      <span className='flex flex-row justify-end w-full pb-16'>
        <button onClick={() => navigate("/stats")} className="text-zinc-500 font-bold mr-4">Stats</button>
        <button onClick={() => signOut(navigate)} className="text-zinc-500 font-bold">Sign out</button>
      </span>

      <Header />
      
      { player == -1 ? // changed from chosenPlayer to player, shouldn't break anything
        // Choose option between rock, paper, scissors
        <ChooseOption choice={choice} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} setError={setError} />
        :
        <Spectate level={level} setWins={setWins} setDraws={setDraws} setLosses={setLosses} setPlayResponse={setPlayResponse} setChoice={setChoice} setError={setError} />
      }

      {error != undefined && <p className="text-xs text-red-400">{error}</p>}
      
      <h1 className={`text-xl tracking-wide font-bold ${playResponse?.result == "win" && "text-green-400"} ${playResponse?.result == "draw" && "text-yellow-400"} ${playResponse?.result == "lose" && "text-red-400"}`}>{playResponse?.result.toUpperCase()} {playResponse != undefined && player != -1 && `(${playerNames[player]})`}</h1> 

      <div className="h-[30rem] flex flex-col md:flex-row md:h-auto md:w-[40rem] items-center justify-evenly">
        <PlayerSide showPlayers={showPlayers} setShowPlayers={setShowPlayers} player={player} chosenLevel={chosenLevel} handleSetPlayer={handleSetPlayer} choice={choice} playerNames={playerNames} />
        <p className="font-bold">vs</p>
        <OpponentSide showLevels={showLevels} setShowLevels={setShowLevels} level={level} chosenPlayer={chosenPlayer} handleSetLevel={handleSetLevel} playResponse={playResponse}/>
      </div>

      { level != undefined && 
        <div className="flex flex-col items-center mt-5">
          <p className="font-bold">{wins} - {draws} - {losses}</p>
          <p className="font-bold">W - D - L</p>
        </div>
      }

      {level != undefined && (wins > 0 || draws > 0 || losses > 0) && <button onClick={() => handleStart(level, player)} className="text-zinc-500 font-bold text-sm mt-10">Reset</button>}
    </div>
  )
}
