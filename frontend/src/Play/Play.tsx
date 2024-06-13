import Header from "../components/Header"
import dropdown from "../assets/dropdown.svg"
import rock from "../assets/rock.svg"
import paper from "../assets/paper.svg"
import scissors from "../assets/scissors.svg"
import { useState } from "react"

export default function Play() {

  const [wins, setWins] = useState(9)
  const [draws, setDraws] = useState(4)
  const [losses, setLosses] = useState(3)

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <span className='flex flex-row justify-end w-full pb-16'>
        <button className="text-zinc-500 font-bold mr-4">Stats</button>
        <button className="text-zinc-500 font-bold">Sign out</button>
      </span>
      <Header />
      <div className="flex flex-row items-center justify-evenly w-full mt-5 mb-5">
        <Option name="Rock" img={rock} />
        <Option name="Paper" img={paper} />
        <Option name="Scissors" img={scissors} />
      </div>
      <div className="h-[30rem] flex flex-col items-center justify-evenly">
        <div className="flex flex-col items-center">
          <button className="mb-3 flex flex-row items-center"><h1 className="font-bold mr-3">Player</h1><img className="w-3 rotate-180" src={dropdown} /></button>
          <Choice name="player" option="" img="" />
        </div>
        <p className="font-bold">vs</p>
        <div className="flex flex-col items-center">
          <button className="mb-3 flex flex-row items-center"><h1 className="font-bold mr-3">Beginner AI</h1><img className="w-3 rotate-180" src={dropdown} /></button>
          <Choice name="ai" option="" img="" />
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
  img: string
}

function Option({ name, img } : OptionProps) {
  return (
    <button className="w-24 h-24 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center">
      <h1 className="font-bold text-xs">{name}</h1>
      <img className="w-8 m-auto" src={img} />
    </button>
  )
}

type ChoiceProps = {
  name: string,
  option: string,
  img: string
}

function Choice({ name, option, img } : ChoiceProps) {
  return (
    <div className="w-40 h-40 bg-[#303030] drop-shadow-xl rounded-sm p-2">
      <img src={img} />
    </div>
  )
}
