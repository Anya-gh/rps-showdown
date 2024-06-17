import Header from "../components/Header";

export default function Credits() {
  return (
    <div className="flex flex-col items-center px-20 py-20 w-screen">
      <Header />
      <h1 className="text-2xl font-bold mt-10 text-left mb-1">Credits</h1>
      <p className="text-xs mb-4">Any items that are not credited here were created by me.</p>
      <ul className="list-disc">
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/rock-paper-scissors" title="rock paper scissors icons">Rock paper scissors icons created by Freepik - Flaticon</a></li>
        <li><a className="text-blue-400" target="_blank" href="https://icons8.com/icon/RlXIngfmfHJq/statistic">Statistic</a> icon by <a className="text-blue-400" target="_blank" href="https://icons8.com">Icons8</a></li>
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/speedometer" title="speedometer icons">Speedometer icons created by Freepik - Flaticon</a></li>
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/eye" title="eye icons">Eye icons created by Gregor Cresnar - Flaticon</a></li>
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/trophy" title="trophy icons">Trophy icons created by Creative Stall Premium - Flaticon</a></li>
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/fire" title="fire icons">Fire icons created by C-mo Box - Flaticon</a></li>
      </ul>
    </div>
  )
}
