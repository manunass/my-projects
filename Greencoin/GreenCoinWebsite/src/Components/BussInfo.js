import React,{useState,useEffect} from 'react'

import {BrowserRouter as Router,Route,Link} from "react-router-dom";
import axios from 'axios';
import {baseUrl} from '../../package.json'


 export default   function BussInfo(props) {
  const [binfo, setbinfo] = useState({Id: "", MunicipalityId: "", Name: "", OwnerFirstName:"", OwnerLastName: "", PhoneNumber: "",Category:"",Address:{
    AreaOrStreet:"",Building:"",Flat:""
  }, Wallet: {Balance: 0,Score:0},About:""})

  useEffect(() => {
    console.log(props)
     axios.get(baseUrl+'/api/Businesses/'+props.ID)
      .then(
      res =>{
        console.log(res);
        setbinfo(res.data);
       

      }
      )
  }, [])
    return (
        <div>      
                <p>ID: {binfo.Id}</p>
                <p>Municipality Id: {binfo.MunicipalityId}</p>
                <p>Name: {binfo.Name}</p>
                <p>Owner Name: {binfo.OwnerFirstName} {binfo.OwnerLastName}</p>
                <p>Phone Number :{binfo.PhoneNumber}</p>
                <p>Category:{binfo.Category}</p>
                <p>Address:</p>
                  <ul>Area or Street :{binfo.Address.AreaOrStreet}</ul>
                  <ul>Building:{binfo.Address.Building}</ul>
                  <ul>Flat :{binfo.Address.Flat}</ul>
                <p>Wallet:{binfo.Wallet.Balance} </p>
                <p>Score:{binfo.Wallet.Score}</p>
                <p>About:{binfo.About}</p>
                <Link to={'/transaction/'+binfo.Wallet.Id} >View Transaction</Link>
        </div>
    )
}
 
