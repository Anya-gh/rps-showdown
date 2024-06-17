import { describe, test, expect, vi, beforeEach, afterEach } from "vitest"
import { render, screen } from "@testing-library/react"
import { MemoryRouter } from "react-router-dom"
import Stats from "../Stats/Stats"

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
  test("performance and analysis buttons on screen", () => {
    // Arrange
    render (
      <MemoryRouter>
        <Stats />
      </MemoryRouter>
    )

    // Act & Assert
    const performanceButton = screen.getByText("Performance")
    expect(performanceButton).not.toBeNull()
    const analysisButton = screen.getByText("Analysis")
    expect(analysisButton).not.toBeNull()
  })
  /* 
  Testing that the correct stats show up on screen requires mocking
  useState due to the way that page is set up. Unfortunately this requires
  changing every useState call to React.useState. I don't want the code
  to be inconsistent by just changing the calls on the stats page and 
  I also don't want to change every call in every page, so I'm
  ommitting the test. It would be very similar in structure to the above
  test.
  */
})