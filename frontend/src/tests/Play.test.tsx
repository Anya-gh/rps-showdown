import { describe, test, expect, vi, beforeEach, afterEach } from "vitest"
import { render, screen } from "@testing-library/react"
import { MemoryRouter } from "react-router-dom"
import Play from "../Play/Play"

const mocks = vi.hoisted(() => {
  return {
    fetchMock: vi.fn(),
    navigateMock: vi.fn(),
    validateUserMock: vi.fn()
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

vi.mock("../components/ValidateUser", () => ({
  default: () => mocks.validateUserMock()
}))

describe("Testing items being on screen", () => {
  beforeEach(() => {
    localStorage.setItem("token", "sdfasdfasdf")
    localStorage.setItem("username", "sdlfjasdfasdf")
    mocks.navigateMock.mockClear()
  })
  afterEach(() => {
    localStorage.removeItem("token")
    localStorage.removeItem("username")
  })
  test("rock, paper and scissors options on screen", () => {
    // Arrange
    render (
      <MemoryRouter>
        <Play />
      </MemoryRouter>
    )
    // Act & Assert
    const rockButton = screen.getByText("rock")
    expect(rockButton).not.toBeNull()
    const paperButton = screen.getByText("paper")
    expect(paperButton).not.toBeNull()
    const scissorsButton = screen.getByText("scissors")
    expect(scissorsButton).not.toBeNull()
  })
})