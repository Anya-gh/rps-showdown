import { describe, test, expect, vi, beforeEach, afterEach } from "vitest"
import { render, screen, fireEvent } from "@testing-library/react"
import Home from "../Home/Home"
import { MemoryRouter } from "react-router-dom"

const mocks = vi.hoisted(() => {
  return {
    fetchMock: vi.fn(),
    navigateMock: vi.fn(),
  }
})

// Mock fetch
global.fetch = mocks.fetchMock

// Mock useNavigate but leave everything else as is from react-router-dom
vi.mock('react-router-dom', async () => {
  const imports = await import('react-router-dom')
  return {
    ...imports,
    useNavigate: () => mocks.navigateMock
  }
})

describe("Login testing", () => {
  beforeEach(() => {
    mocks.navigateMock.mockClear()
  })

  test("shows error message is no username", () => {
    // Arrange
    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    )

    // Act
    const passwordField = screen.getByPlaceholderText("Password...")
    expect(passwordField).not.toBeNull()
    fireEvent.change(passwordField, {target: {value: "Password"}})
    const loginButton = screen.getByText("Login / Register")
    expect(loginButton).not.toBeNull()
    fireEvent.click(loginButton);
    const error = screen.getByText("Please enter your username.")

    // Assert
    expect(error).not.toBeNull()
  })

  test("if username and password are set a request is made to login", () => {
    // Arrange
    render(
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    )

    // Act
    const usernameField = screen.getByPlaceholderText("Username...")
    expect(usernameField).not.toBeNull()
    fireEvent.change(usernameField, {target: {value: "ABC"}})
    const passwordField = screen.getByPlaceholderText("Password...")
    expect(passwordField).not.toBeNull()
    fireEvent.change(passwordField, {target: {value: "DEF"}})
    const loginButton = screen.getByText("Login / Register")
    expect(loginButton).not.toBeNull()
    fireEvent.click(loginButton);

    // Assert
    expect(mocks.fetchMock).toHaveBeenCalled()
  })
})

describe("Play or Stats testing", () => {
  beforeEach(() => {
    localStorage.setItem("token", "sdfasdfasdf")
    localStorage.setItem("username", "sdlfjasdfasdf")
    mocks.navigateMock.mockClear()
  })
  afterEach(() => {
    localStorage.removeItem("token")
    localStorage.removeItem("username")
  })
  test("play or stats buttons shows up on screen when there is a valid token and username", () => {
    // Arrange
    render (
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    )

    // Act & Assert
    const playButton = screen.getByText("PLAY")
    expect(playButton).not.toBeNull()
    const statsButton = screen.getByText("STATS")
    expect(statsButton).not.toBeNull()
  })
  test("play navigates to /play", () => {
    // Arrange
    render (
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    )

    // Act
    const playButton = screen.getByText("PLAY")
    expect(playButton).not.toBeNull()
    fireEvent.click(playButton)

    // Assert
    expect(mocks.navigateMock).toHaveBeenCalledWith("/play")
  })
  test("stats navigates to /stats", () => {
    // Arrange
    render (
      <MemoryRouter>
        <Home />
      </MemoryRouter>
    )

    // Act
    const statsButton = screen.getByText("STATS")
    expect(statsButton).not.toBeNull()
    fireEvent.click(statsButton)

    // Assert
    expect(mocks.navigateMock).toHaveBeenCalledWith("/stats")
  })
})