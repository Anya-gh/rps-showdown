import { useState, useEffect } from "react"
import rockIcon from "../assets/rock.svg"
import paperIcon from "../assets/paper.svg"
import scissorsIcon from "../assets/scissors.svg"

type ChoiceProps = {
  choice: string | undefined
}

function Choice({ choice } : ChoiceProps) {

  const [img, setImg] = useState<string>()

  useEffect(() => {
    if (choice == "rock") { setImg(rockIcon) }
    else if (choice == "paper") { setImg(paperIcon) }
    else if (choice == "scissors") { setImg(scissorsIcon) }
    else { setImg(undefined) }
  }, [choice])

  return (
    <div className="w-40 h-40 bg-[#303030] drop-shadow-xl rounded-sm p-2 flex flex-col items-center justify-center">
      <img className="w-20" src={img} />
    </div>
  )
}

export default Choice;