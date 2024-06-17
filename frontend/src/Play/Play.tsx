import Header from "../components/Header"
import dropdown from "../assets/dropdown.svg"
import rock from "../assets/rock.svg"
import paper from "../assets/paper.svg"
import scissors from "../assets/scissors.svg"
import { useState, useEffect, Dispatch, SetStateAction } from "react"
import { useNavigate } from "react-router-dom"
import ValidateUser from "../components/ValidateUser"
import Modal from "react-modal"
import { PlayType, SpectateType } from "../Types"

/*
TODO:
- navigation
- using real functionality
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
  const levelNames = ["Beginner", "Intermediate", "Advanced"]
  const [showLevels, setShowLevels] = useState(false)
  const [level, setLevel] = useState<number | undefined>()
  const [chosenLevel, setChosenLevel] = useState<number | undefined>(level)
  const playerNames = ["Player", "Beginner", "Intermediate", "Advanced"]
  const [player, setPlayer] = useState<number>(-1)
  const [showPlayers, setShowPlayers] = useState(false)
  const [chosenPlayer, setChosenPlayer] = useState<number>(player)
  const [modalOpen, setModalOpen] = useState(false)

  const handleSetPlayer = (newChosenPlayer: number) => {
    const actualChosenPlayer = newChosenPlayer == 0 ? -1 : newChosenPlayer
    setChosenPlayer(actualChosenPlayer)
    setShowPlayers(false)
    if (level == undefined) { 
      // If a level hasn't been selected, freely change player without creating a session.
      setPlayer(actualChosenPlayer)
    }
    else {
      setModalOpen(true)
    }
  }

  const handleSetLevel = (newChosenLevel: number) => {
    setChosenLevel(newChosenLevel);
    setShowLevels(false);
    // Only relevant if level is still "Select a level"; player can't be changed until a level is selected.
    if (level == undefined) { handleStart(newChosenLevel, chosenPlayer) } 
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

  const handleSpectate = async () => {
    console.log(JSON.stringify({
      "Username" : localStorage.getItem("username"),
      "SessionID" : localStorage.getItem("session")
    }))
    if (level == undefined) { return; } // show an error message
    const request = await fetch("http://localhost:5000/spectate", {
      method: "POST",
      headers: {
        "Content-Type" : "application/json",
        "Authorization" : `bearer ${localStorage.getItem("token")}`
      },
      body : JSON.stringify({
        "Username" : localStorage.getItem("username"),
        "SessionID" : localStorage.getItem("session")
      })
    })

    if (!request.ok) { throw new Error(request.statusText) }
    const response = await request.json() as SpectateType
    if (response.result == "win") { setWins(wins => wins+1) }
    else if (response.result == "draw") { setDraws(draws => draws+1) }
    else if (response.result == "lose") { setLosses(losses => losses+1) }
    setPlayResponse({botChoice: response.levelChoice, result: response.result})
    setChoice(response.playerChoice)
  }

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <Modal
        isOpen={modalOpen}
        onRequestClose={() => setModalOpen(false)}
        className="w-screen h-screen flex flex-col items-center justify-center bg-black opacity-70"
      >
        <div>
          <div className="w-60 bg-[#303030] text-center rounded-xl p-3">
            <p className="text-sm mb-3">Are you sure you want to change who's playing? Any bots will be reset and will not be able to see any past moves thus far.</p>
            <div className="flex flex-row items-center justify-evenly">
              <button onClick={() => handleStart(chosenLevel, chosenPlayer)} className="py-1 px-3 bg-blue-500 rounded-lg">Yes</button>
              <button onClick={handleCancel} className="p-1 px-3 bg-red-500 rounded-lg">No</button>
            </div>
          </div>
        </div>
      </Modal>
      <span className='flex flex-row justify-end w-full pb-16'>
        <button className="text-zinc-500 font-bold mr-4">Stats</button>
        <button className="text-zinc-500 font-bold">Sign out</button>
      </span>
      <Header />
      { chosenPlayer == -1 ?
        <div className="flex flex-row items-center justify-evenly w-full mt-5 mb-5 md:w-[40rem]">
          <Option name="rock" choice={choice} img={rock} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} />
          <Option name="paper" choice={choice} img={paper} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} />
          <Option name="scissors" choice={choice} img={scissors} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} />
        </div>
        :
        <button onClick={handleSpectate} className="w-24 h-24 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center mt-5 mb-5">
          <h1 className="font-bold text-xs capitalize mb-3">Play</h1>
          {/* replace with spectate icon */}
          <p className='text-xs text-zinc-400'>Spectate a round.</p>
        </button>
      }
      { chosenPlayer == -1 && <h1 className={`text-xl tracking-wide font-bold ${playResponse?.result == "win" && "text-green-400"} ${playResponse?.result == "draw" && "text-yellow-400"} ${playResponse?.result == "lose" && "text-red-400"}`}>{playResponse?.result.toUpperCase()}</h1>

      }
      <div className="h-[30rem] flex flex-col md:flex-row md:h-auto md:w-[40rem] items-center justify-evenly">
        <div className="flex flex-col items-center">
          <div>
            <button onClick={() => setShowPlayers(prev => !prev)} className="mb-3 flex flex-row items-center"><h1 className="font-bold mr-3">{player == -1 ? playerNames[0] : playerNames[player]}</h1><img className={`w-3 transition duration-200 ${!showPlayers && "rotate-180"}`} src={dropdown} /></button>
          </div>
          {showPlayers && 
              <div className="absolute mt-12 z-10">
                <ul className="bg-[#202020] rounded-xl p-2 flex flex-col w-32 items-start">
                {playerNames.map((playerName, index) => {
                  if ((index != player) && !(player == -1 && index == 0) && (chosenLevel == undefined || (index != chosenLevel+1))) return (<button key={playerName} onClick={() => handleSetPlayer(index)} ><h1>{playerName}</h1></button>)
                })}
                </ul>
              </div>
            }
          <Choice choice={choice} />
        </div>
        <p className="font-bold">vs</p>
        <div className="flex flex-col items-center">
          <div>
            <button onClick={() => setShowLevels(prev => !prev)} className="mb-3 flex flex-row items-center"><h1 className="font-bold mr-3">{level != undefined ? levelNames[level] : "Select a level"}</h1><img className={`w-3 transition duration-200 ${!showLevels && "rotate-180"}`} src={dropdown} /></button>
            {showLevels && 
              <div className="absolute mt-3 z-10">
                <ul className="bg-[#202020] rounded-xl p-2 flex flex-col w-32 items-start">
                {levelNames.map((levelName, index) => {
                  if ((index != level) && (chosenPlayer == -1 || (chosenPlayer != -1 && index != chosenPlayer-1))) return (<button key={levelName} onClick={() => handleSetLevel(index)}><h1>{levelName}</h1></button>)
                })}
                </ul>
              </div>
            }
          </div>
          <Choice choice={playResponse?.botChoice} />
        </div>
      </div>
      <div className="flex flex-col items-center mt-5">
          <p className="font-bold">{wins} - {draws} - {losses}</p>
          <p className="font-bold">W - D - L</p>
        </div>
      <button className="text-zinc-500 font-bold text-sm mt-10">Reset</button>
    </div>
  )
}

type OptionProps = {
  name: string,
  choice: string | undefined,
  img: string,
  setChoice: Dispatch<SetStateAction<string | undefined>>,
  setPlayResponse: Dispatch<SetStateAction<PlayType | undefined>>,
  setWins: Dispatch<SetStateAction<number>>,
  setDraws: Dispatch<SetStateAction<number>>,
  setLosses: Dispatch<SetStateAction<number>>,
  level: number | undefined
}

function Option({ name, choice, img, setChoice, setPlayResponse, setWins, setDraws, setLosses, level } : OptionProps) {


  const handleOption = async () => {
    console.log(JSON.stringify({
      "Username" : localStorage.getItem("username"),
      "PlayerChoice" : name,
      "SessionID" : localStorage.getItem("session")
    }))
    if (level == undefined) { return; } // show an error message
    const request = await fetch("http://localhost:5000/play", {
      method: "POST",
      headers: {
        "Content-Type" : "application/json",
        "Authorization" : `bearer ${localStorage.getItem("token")}`
      },
      body : JSON.stringify({
        "Username" : localStorage.getItem("username"),
        "PlayerChoice" : name,
        "SessionID" : localStorage.getItem("session")
      })
    })

    if (!request.ok) { throw new Error(request.statusText) }
    const response = await request.json() as PlayType
    if (response.result == "win") { setWins(wins => wins+1) }
    else if (response.result == "draw") { setDraws(draws => draws+1) }
    else if (response.result == "lose") { setLosses(losses => losses+1) }
    setPlayResponse(response)
    setChoice(name)
  }

  return (
    <button onClick={handleOption} className="w-24 h-24 transition duration-200 md:hover:scale-110 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center">
      <h1 className={`font-bold text-xs capitalize ${choice == name && "text-blue-400"}`}>{name}</h1>
      <img className="w-8 m-auto" src={img} />
    </button>
  )
}

type ChoiceProps = {
  choice: string | undefined
}

function Choice({ choice } : ChoiceProps) {

  const [img, setImg] = useState<string>()

  useEffect(() => {
    if (choice == "rock") { setImg(rock) }
    else if (choice == "paper") { setImg(paper) }
    else if (choice == "scissors") { setImg(scissors) }
    else { setImg(undefined) }
  }, [choice])

  return (
    <div className="w-40 h-40 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center justify-center">
      <img className="w-20" src={img} />
    </div>
  )
}
