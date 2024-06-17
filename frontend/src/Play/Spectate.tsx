import { Dispatch, SetStateAction } from "react";
import { PlayType, SpectateType } from "../Types";
import spectateIcon from "../assets/spectate.png"

type SpectateProps = {
  level: number | undefined,
  setWins: Dispatch<SetStateAction<number>>,
  setDraws: Dispatch<SetStateAction<number>>,
  setLosses: Dispatch<SetStateAction<number>>,
  setPlayResponse: Dispatch<SetStateAction<PlayType | undefined>>,
  setChoice: Dispatch<SetStateAction<string | undefined>>
}

function Spectate({ level, setWins, setDraws, setLosses, setPlayResponse, setChoice } : SpectateProps) {

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
    <button onClick={handleSpectate} className="w-24 h-24 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center mt-5 mb-5">
      <h1 className="font-bold text-xs capitalize mb-1">Play</h1>
      <p className='text-[0.5rem] text-zinc-400 mb-2'>Spectate a round.</p>
      <img className='w-8' src={spectateIcon} />
    </button>
  )
}

export default Spectate;