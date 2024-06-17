import { Dispatch, SetStateAction } from "react"
import dropdown from "../assets/dropdown.svg"
import Choice from "./Choice"

type PlayerSideProps = {
  showPlayers: boolean,
  setShowPlayers: Dispatch<SetStateAction<boolean>>
  player: number,
  chosenLevel: number | undefined,
  handleSetPlayer: (player: number) => void,
  choice: string | undefined,
  playerNames: string[]
}

function PlayerSide({ showPlayers, setShowPlayers, player, chosenLevel, handleSetPlayer, choice, playerNames } : PlayerSideProps) {
    
  return (
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
  )
}

export default PlayerSide