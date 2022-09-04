import React, { Component } from 'react'
import '../css/About.css'

class About extends Component {
  render() {
    return (
      <div className='about-section'>
        <p>
          BakedEnv is a scripting language made in C#, for C#. Designed after Lua, it is built 
          for extensibility between C# and written scripts.
        </p>

        <p>
          "The Lua of C#" as I like to call it. BakedEnv can sate all of your needs for an 
          embedded scripting language in C#. From game nodding to your own mini scripting 
          language, it is applicable to a wide range of uses. BakedEnv is focused to be as 
          extensible as possible, allowing for the creation, implementation, and modification 
          of core behavior. Additionally, the ease of access for cross-language typing via 
          functions and objects contribute to its applicability.
        </p>
      </div>
    )
  }
}

export default About