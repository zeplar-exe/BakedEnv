import './App.css';
import './layouts/MainLayout'
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import About from './pages/About';
import Reference from './pages/reference/Reference';
import Documentation from './pages/documentation/Documentation';

function App() {
  return (
    <Router>
      <MainLayout>
        <Routes>
          <Route path='/' element={<Home></Home>}/>
          <Route path='about' element={<About></About>}/>
          <Route path='reference' element={<Reference></Reference>}/>
          <Route path='documentation' element={<Documentation></Documentation>}/>
        </Routes>
      </MainLayout>
    </Router>
  );
}

export default App;
