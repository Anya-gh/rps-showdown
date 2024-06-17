import Header from "../components/Header";
import { useState, useEffect } from "react";
import { StatsType } from "../Types";
import { useNavigate } from "react-router-dom";
import ValidateUser from "../components/ValidateUser";
import Performance from "./Performance";
import Analysis from "./Analysis";
import signOut from "../components/SignOut";
import Levels from "./Levels"

/*
TODO:
- navigation
- using real values
*/

export default function Stats() {

  const [userStats, setUserStats] = useState<StatsType[]>();
  const [level, setLevel] = useState(0);
  const navigate = useNavigate();
  useEffect(() => {
    ValidateUser(navigate);

    fetchStats();
    async function fetchStats() {
      const request = await fetch("http://localhost:5000/stats", {
        method: "POST",
        headers: { 
          "Content-Type" : "application/json",
          "Authorization" : `bearer ${localStorage.getItem("token")}` // Must exist because of ValidateUser
        },
        body: JSON.stringify({"Username" : localStorage.getItem("username")})
      })
      if (!request.ok) {
        throw new Error(request.statusText);
      }
      else {
        const response = await request.json() as StatsType[]
        setUserStats(response);
      }
    }
  }, [])

  const [showPerformance, setShowPerformance] = useState(true)

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <span className='flex flex-row justify-end w-full pb-16'>
        <button onClick={() => {navigate("/play")}} className="text-zinc-500 font-bold mr-4">Play</button>
        <button onClick={() => signOut(navigate)} className="text-zinc-500 font-bold">Sign out</button>
      </span>
      <Header />
      <Levels level={level} setLevel={setLevel}/>
      <span className='flex flex-row w-3/4 justify-evenly mt-5 md:mb-10'>
        <button className={`font-bold p-3 bg-[#303030] drop-shadow-xl rounded-xl transition duration-200 md:hover:scale-110 ${showPerformance ? "text-white" : "text-zinc-500"}`} onClick={() => setShowPerformance(true)}>Performance</button>
        <button className={`font-bold p-3 bg-[#303030] drop-shadow-xl rounded-xl transition duration-200 md:hover:scale-110 ${showPerformance ? "text-zinc-500" : "text-white"}`} onClick={() => setShowPerformance(false)}>Analysis</button>
      </span>
      {userStats && (showPerformance ? <Performance userStats={userStats[level]}/> : <Analysis userStats={userStats[level]}/>)}
    </div>
  )
}