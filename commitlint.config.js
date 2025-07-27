module.exports = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'scope-enum': [2, 'always', [
      'GameEngine.Core',
      'GameEngine.Physics',
      'GameEngine.IO',
      'GameEngine.Graphics',
      // Add all your package scopes here exactly as in your release-please config
    ]],
    'type-enum': [2, 'always', [
      'feat',
      'fix',
      'chore',
      'docs',
      'style',
      'refactor',
      'perf',
      'test',
      // Add any other commit types you use
    ]],
  },
};