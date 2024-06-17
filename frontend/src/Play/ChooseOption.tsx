import { Dispatch, SetStateAction } from "react";
import { PlayType } from "../Types";
import rockIcon from "../assets/rock.svg"
import paperIcon from "../assets/paper.svg"
import scissorsIcon from "../assets/scissors.svg"

type BaseProps = {
  choice: string | undefined,
  setChoice: Dispatch<SetStateAction<string | undefined>>,
  setPlayResponse: Dispatch<SetStateAction<PlayType | undefined>>,
  setWins: Dispatch<SetStateAction<number>>,
  setDraws: Dispatch<SetStateAction<number>>,
  setLosses: Dispatch<SetStateAction<number>>,
  level: number | undefined,
  setError: Dispatch<SetStateAction<string | undefined>>
}

type ChooseOptionProps = BaseProps

function ChooseOption({ choice, setChoice, setPlayResponse, setWins, setDraws, setLosses, level, setError } : ChooseOptionProps) {
  return (
    <div className="flex flex-row items-center justify-evenly w-full mt-5 mb-5 md:w-[40rem]">
      <Option name="rock" choice={choice} img={rockIcon} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} setError={setError} />
      <Option name="paper" choice={choice} img={paperIcon} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} setError={setError} />
      <Option name="scissors" choice={choice} img={scissorsIcon} setChoice={setChoice} setPlayResponse={setPlayResponse} setWins={setWins} setDraws={setDraws} setLosses={setLosses} level={level} setError={setError} />
    </div>
  )
}

type OptionProps = BaseProps & { name: string, img: string }

function Option({ name, choice, img, setChoice, setPlayResponse, setWins, setDraws, setLosses, level, setError } : OptionProps) {


  const handleOption = async () => {
    setError(undefined)
    console.log(JSON.stringify({
      "Username" : localStorage.getItem("username"),
      "PlayerChoice" : name,
      "SessionID" : localStorage.getItem("session")
    }))
    if (level == undefined) { 
      setError("Select a level first!")
      return; 
    }
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

export default ChooseOption;