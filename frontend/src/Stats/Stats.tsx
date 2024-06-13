import Header from "../components/Header";
import dropdown from "../assets/dropdown.svg"
import rock from "../assets/rock.svg"
import paper from "../assets/paper.svg"
import scissors from "../assets/scissors.svg"
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

/*
TODO:
- navigation
- using real values
*/

export default function Stats() {

  const navigate = useNavigate()

  useEffect(() => {
    if (!localStorage.getItem("token")) {
      navigate('/')
    }
  })

  const [showPerformance, setShowPerformance] = useState(true)

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <span className='flex flex-row justify-end w-full pb-16'>
        <button className="text-zinc-500 font-bold mr-4">Play</button>
        <button className="text-zinc-500 font-bold">Sign out</button>
      </span>
      <Header />
      <div className="flex flex-row items-center w-3/4 justify-evenly font-bold mt-5">
        <h1>Player</h1>
        <p>vs</p>
        <button className="flex flex-row items-center">
          <h1 className="mr-3">Beginner</h1>
          <img className="w-3 rotate-180" src={dropdown} />
        </button>
      </div>
      <span className='flex flex-row w-3/4 justify-between mt-5'>
        <button className="font-bold" onClick={() => setShowPerformance(true)}>Performance</button>
        <button className="font-bold" onClick={() => setShowPerformance(false)}>Analysis</button>
      </span>
      {showPerformance ? <Performance /> : <Analysis />}
    </div>
  )
}

function Performance() {

  const [winrate, setWinrate] = useState(76)
  const [streak, setStreak] = useState(8)

  return (
    <div className="mt-5 w-full flex flex-col items-center">
      <span className="flex flex-col items-center font-bold mb-5">
        <h1 className="text mb-3">Win Rate</h1>
        <p className="text-3xl">{winrate} %</p>
      </span>
      <span className="flex flex-col items-center font-bold mb-5">
        <h1 className="text mb-3">Longest Win Streak</h1>
        <p className="text-3xl">{streak} Wins</p>
      </span>
      <Leaderboard />
    </div>
  )

  function Leaderboard() {

    type leaderboardStats = {
      username: string,
      winrate: number
    }

    // map over Anya 96 % bit
    const [leaderboardEntries, setLeaderboardEntries] = useState<leaderboardStats[]>([{"username" : "Anya", "winrate" : 96}, {"username" : "Rierra", "winrate" : 96}])

    return (
      <div className='w-full flex flex-col items-center'>
        <h1 className="font-bold mb-5">Leaderboard</h1>
        <table className='text-left w-3/4 border-collapse border border-zinc-400 bg-[#404040] rounded-sm'>
          <thead>
            <tr className="font-bold italic text-zinc-200 text-sm">
              <th className="border border-zinc-400 p-1">Username</th>
              <th className="border border-zinc-400 p-1">Win Rate</th>
            </tr>
          </thead>
          <tbody>
            {leaderboardEntries.map(entry => {
              return (
                <tr className="font-bold">
                  <td className="border border-zinc-400 p-1">{entry.username}</td>
                  <td className="border border-zinc-400 p-1">{entry.winrate} %</td>
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>
    )
  }
}

function Analysis() {

  const [playstyle, setPlaystyle] = useState("Aggressive")
  const [ace, setAce] = useState(rock)
  const [nemesis, setNemesis] = useState(scissors)

  return (
    <div className="w-full mt-5">
      <div className="flex flex-col items-center justify-evenly text-lg font-bold">
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-5">Choice Distribution</h1>
          <img className="w-24 h-24" src="" />
        </span>
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-5">Ace</h1>
          <img className="w-24" src={ace} />
        </span>
        <span className="flex flex-col items-center mb-10">
          <h1 className="mb-5">Nemesis</h1>
          <img className="w-24" src={nemesis} />
        </span>
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-10">Playstyle</h1>
          <p className="text-3xl">{playstyle}</p>
        </span>
      </div>
    </div>
  )
}