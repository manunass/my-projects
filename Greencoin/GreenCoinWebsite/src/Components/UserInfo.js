import React,{useState,useEffect} from 'react'
import {BrowserRouter as Router,Route,Link} from "react-router-dom";
import axios from 'axios';
import {baseUrl} from '../../package.json'


export default function UserInfo(props) {
  const [wallet,setwallet]=useState(true);
  const [data,setdata]=useState('');
    const [userinfo, setuserinfo] = useState({Id: "", MunicipalityId: "", Name: "", Email: "", PhoneNumber: "",Address:{
        AreaOrStreet:"",Building:"",Flat:""
      }, Wallet: {Id:"",Balance: 0,Score:0}})
    
      useEffect(() => {
        console.log(props)
         axios.get(baseUrl+'/api/Users/'+props.ID)
          .then(
          res =>{
            console.log(res);
            setuserinfo(res.data);
            setwallet(false)
            
          }
          )
      }, [])
    
    return (
      
             <div> 
               {!wallet ? 
               <div>    
                <p>ID: {userinfo.Id}</p>
                <p>Municipality Id: {userinfo.MunicipalityId}</p>
                <p>Name: {userinfo.FirstName} {userinfo.LastName}</p>
                <p>Email:{userinfo.Email}</p>
                <p>Phone Number :{userinfo.PhoneNumber}</p>
                <p>Address:</p>
                  <ul>Area or Street :{userinfo.Address.AreaOrStreet}</ul>
                  <ul>Building:{userinfo.Address.Building}</ul>
                  <ul>Flat :{userinfo.Address.Flat}</ul>
                <p>Wallet:{userinfo.Wallet.Balance} </p>
                <p>Score:{userinfo.Wallet.Score}</p>
                <Link to={'/transaction/'+userinfo.Wallet.Id} >View Transaction</Link>
        </div> 
        :" "}
        
        
        </div>
        
    )
}

