import { Dispatch, SetStateAction } from "react";
import playButtonIcon from "../assets/play-button.png"
import statsButtonIcon from "../assets/stats-button.png"
import { useNavigate } from "react-router-dom";

type PlayOrStatsProps = {
  setLoggedIn: Dispatch<SetStateAction<boolean>>
}

function PlayOrStats({ setLoggedIn } : PlayOrStatsProps) {

  const navigate = useNavigate()

  const signOutHandler = () => {
    setLoggedIn(false);
    localStorage.removeItem("token")
    localStorage.removeItem("username")
  }

  return (
    <div className="w-full flex flex-col items-center my-5">
      <div className="flex flex-row items-center w-full justify-evenly mb-5">
        <button onClick={() => navigate('/play')} className="w-32 h-32 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center md:hover:scale-110 transition duration-200">
          <h1 className="text-lg italic font-bold text-center mb-3">PLAY</h1>
          <img className="w-12 h-12" src={playButtonIcon} />
        </button>
        <button onClick={() => navigate('/stats')} className="w-32 h-32 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center md:hover:scale-110 transition duration-200">
          <h1 className="text-lg italic font-bold text-center mb-3">STATS</h1>
          <img className="w-10 h-10" src={statsButtonIcon} />
        </button>
      </div>
      <button onClick={signOutHandler} className="py-1 px-3 rounded-xl bg-blue-600 text-xs mb-2 md:hover:scale-105 transition duration-200">Sign out</button>
    </div>
  )
}

export default PlayOrStats;