import { SetStateAction, Dispatch, useState } from "react"
import dropdown from "../assets/dropdown.svg"

type LevelsProps = {
  level: number,
  setLevel: Dispatch<SetStateAction<number>>
}

function Levels({ level, setLevel } : LevelsProps) {

  const [showLevels, setShowLevels] = useState(false)
  const levelNames = ["Beginner", "Intermediate", "Advanced"]

  return (
    <div className="flex flex-col items-center w-3/4 md:w-1/2 justify-center font-bold mt-5">
        <button onClick={() => setShowLevels(prev => !prev)} className="flex flex-row items-center">
          <h1 className="mr-3">Player vs {levelNames[level]}</h1>
          <img className={`w-3 transition duration-200 ${!showLevels && "rotate-180"}`} src={dropdown} />
        </button>
        {showLevels && 
          <div className="absolute mt-28 z-10">
            <ul className="bg-[#202020] rounded-xl p-2 flex flex-col w-32 items-start">
              {levelNames.map((levelName, index) => {
                if (index != level) return (<button key={levelName} onClick={() => {setShowLevels(false); setLevel(index)}}><h1>{levelName}</h1></button>)
              })}
            </ul>
          </div>
        }
    </div>
  )
}

export default Levels;