import { Dispatch, SetStateAction } from "react";
import playButtonIcon from "../assets/play-button.png"
import statsButtonIcon from "../assets/stats-button.png"
import { useNavigate } from "react-router-dom";
import signOut from "../components/SignOut";

type PlayOrStatsProps = {
  setLoggedIn: Dispatch<SetStateAction<boolean>>
}

function PlayOrStats({ setLoggedIn } : PlayOrStatsProps) {

  const navigate = useNavigate()

  const signOutHandler = () => {
    setLoggedIn(false);
    signOut(navigate);
  }

  return (
    <div className="w-full md:w-[40rem] flex flex-col items-center my-5 md:mt-5 md:mb-10">
      <div className="flex flex-row items-center w-full justify-evenly mb-5 md:mb-10">
        <button onClick={() => navigate('/play')} className="w-32 h-32 md:w-60 md:h-60 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center md:hover:scale-110 transition duration-200">
          <h1 className="text-lg italic font-bold text-center mb-3 md:text-2xl">PLAY</h1>
          <img className="w-12 h-12 md:w-32 md:h-32" src={playButtonIcon} />
        </button>
        <button onClick={() => navigate('/stats')} className="w-32 h-32 md:w-60 md:h-60 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center md:hover:scale-110 transition duration-200">
          <h1 className="text-lg italic font-bold text-center mb-3 md:text-2xl md:mb-6">STATS</h1>
          <img className="w-10 h-10 md:w-28 md:h-28" src={statsButtonIcon} />
        </button>
      </div>
      <button onClick={signOutHandler} className="py-1 px-3 rounded-xl bg-blue-600 text-xs mb-2 md:hover:scale-105 transition duration-200">Sign out</button>
    </div>
  )
}

export default PlayOrStats;