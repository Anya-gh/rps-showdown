import { NavigateFunction } from "react-router-dom"

function ValidateUser(navigate: NavigateFunction) {

  const token = localStorage.getItem("token")
  const username = localStorage.getItem("username")
  if (token && username) { validateUser() }
  else { navigate('/') }

  async function validateUser() {
    const request = await fetch("http://localhost:5000/", {
      method: "GET",
      headers: {
        "ContentType" : "application/json"
      }
    })
    if (!request.ok) { navigate('/') }
  }
}

export default ValidateUser;