import { NavigateFunction } from "react-router-dom"

const signOut = (navigate: NavigateFunction) => {
  localStorage.removeItem("token")
  localStorage.removeItem("username")
  navigate("/")
}

export default signOut