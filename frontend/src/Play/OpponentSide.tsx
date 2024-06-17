import { Dispatch, SetStateAction } from "react"
import { PlayType } from "../Types"
import dropdown from "../assets/dropdown.svg"
import Choice from "./Choice"

type OpponentSideProps = {
  showLevels: boolean,
  setShowLevels: Dispatch<SetStateAction<boolean>>,
  level: number | undefined,
  chosenPlayer: number,
  handleSetLevel: (level: number) => void,
  playResponse: PlayType | undefined
}

function OpponentSide({ showLevels, setShowLevels, level, chosenPlayer, handleSetLevel, playResponse } : OpponentSideProps) {

  const levelNames = ["Beginner", "Intermediate", "Advanced"]

  return (
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
      <Choice choice={playResponse?.levelChoice} />
    </div>
  )
}

export default OpponentSide;