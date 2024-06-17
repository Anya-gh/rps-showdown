import { StatsType } from "../Types";
import Card from "./Card";
import winRateIcon from "../assets/win-rate.png"
import streakIcon from "../assets/streak.png"

type PerformanceProps = {
  userStats: StatsType
}

function Performance({ userStats } : PerformanceProps) {

  return (
    <div className="mt-5 w-full flex flex-col md:flex-row md:justify-evenly md:w-2/3 items-center">
      <Card>
        <div className="flex flex-col items-center font-bold justify-center h-full">
          <span className="flex flex-row items-center mb-auto">
            <img className="w-6 h-6 mr-3" src={winRateIcon} />
            <h1 className="text-lg">Win Rate</h1>
          </span>
          <p className="text-5xl mb-auto">{Math.round(userStats.winRate*100)} %</p>
        </div>
      </Card>
      <Card>
        <div className="flex flex-col items-center font-bold justify-center h-full">
          <span className="flex flex-row items-center mb-auto">
            <img className="w-6 h-6 mr-3" src={streakIcon} />
            <h1 className="text-lg">Longest Win Streak</h1>
          </span>
          <p className="text-5xl mb-auto">{userStats.longestStreak} Wins</p>
        </div>
      </Card>
      {/*<Leaderboard />*/}
    </div>
  )
}

export default Performance;

/*function Leaderboard() {

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
  }*/