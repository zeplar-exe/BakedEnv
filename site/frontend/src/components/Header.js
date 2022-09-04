import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import '../css/Header.css'
import gh_logo from '../assets/GitHub-Mark/PNG/GitHub-Mark-32px.png'

export default class Header extends Component {
  render() {
    return (
      <div className='header'>
        <Link id='home-link' to="/">BakedEnv</Link>
        <Link className='header-item' to="/about">About</Link>
        <Link className='header-item' to="/reference">Language Reference</Link>
        <Link className='header-item' to="/documentation">Documentation</Link>
        <a className='header-item' target="_blank" rel="noopener noreferrer" href="https://github.com/zeplar-exe/BakedEnv/">
          <img src={gh_logo} alt='GitHub'></img>
        </a>
      </div>
    )
  }
}