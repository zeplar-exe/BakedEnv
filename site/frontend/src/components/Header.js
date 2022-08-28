import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import '../css/Header.css'

export default class Header extends Component {
  render() {
    return (
      <div className='header'>
        <Link id='home-link' to="/">BakedEnv</Link>
        <div className='spacer-1'></div>
        <Link className='link' to="/reference">Language Reference</Link>
        <hr className='divider'/>
        <Link className='link' to="/documentation">Documentation</Link>
      </div>
    )
  }
}