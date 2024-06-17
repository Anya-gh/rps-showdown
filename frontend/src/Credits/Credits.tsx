import Header from "../components/Header";

export default function Credits() {
  return (
    <div className="flex flex-col items-center px-20 py-20 w-screen">
      <Header />
      <h1 className="text-2xl font-bold mt-10 text-left mb-2">Credits</h1>
      <ul className="list-disc">
        <li><a className="text-blue-400" href="https://www.flaticon.com/free-icons/rock-paper-scissors" title="rock paper scissors icons">Rock paper scissors icons created by Freepik - Flaticon</a></li>
      </ul>
    </div>
  )
}
