const PROXY_CONFIG = [
  {
      context: [
          "/api"
      ],
      target: "https://localhost:6001",
      secure: true
  }
]

module.exports = PROXY_CONFIG;