import React,{useEffect,useState} from 'react'
import { MDBCard, MDBCardBody, MDBCardImage, MDBCardText, MDBRow,MDBBadge} from
'mdbreact';
import {RiCoinLine} from "react-icons/ri";
 import {FcHome,FcLike,FcProcess,FcAnswers} from "react-icons/fc"
import { Col } from 'react-bootstrap';
import axios from 'axios'
import {baseUrl} from '../../package.json'


export default function Dashboard() {
  
        const [profile, setprofile] = useState(null);
        const [loading, setloading] = useState(true);
        
        var config = {
            headers: {'Access-Control-Allow-Origin': '*'}
        };
        useEffect(
          () =>{
              
              axios.get(baseUrl+'/api/Municipalities/0713b854-89ef-497b-a61c-e0bdb669fa70',config).then(
                  res =>{
                    setprofile(res.data);
                    setloading(false);
                   console.log(profile);
                  }
              )
              .catch(
                  err =>{
                      console.log(err);
                  }
              )
              
               
                
               
    }, []);
    if(loading) return  <div className="spinner-grow text-primary" role="status">
    <span className="sr-only">Loading...</span>
  </div>;
   

    return (
        <div>
              
            <MDBRow className="row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4">
                    <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage
                        className='narrow winter-neva-gradient '
                        
                        tag='div'
                    >
                     {<FcLike />} 
                    </MDBCardImage>
                    <MDBCardBody >
                        <MDBCardText >
                        Users  
                        <MDBBadge className="ml-3" color="light">200</MDBBadge>
                        </MDBCardText> 
                    </MDBCardBody>
                    </MDBCard>
                   </Col>
                   <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage
                        className='narrow winter-neva-gradient '
                        
                        tag='div'
                    >
                        {<FcHome/>}  
                    </MDBCardImage>
                    <MDBCardBody cascade className='text-center'>
                        <MDBCardText>
                      Recyclables Revenue 
                      <MDBBadge className="ml-3" color="light">3000</MDBBadge>
                        </MDBCardText>
                    </MDBCardBody>
                    </MDBCard>
                   </Col>
                   <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage
                        className='narrow winter-neva-gradient '
                        
                        tag='div'
                    >
                        <FcProcess/>
                        
                    </MDBCardImage>
                    <MDBCardBody cascade className='text-center'>
                        <MDBCardText>
                        Coins in Circulation
                        <MDBBadge className="ml-3" color="light">{profile.CoinsInCirculation}</MDBBadge>
                        </MDBCardText>
                    </MDBCardBody>
                    </MDBCard>
                   </Col>
                   <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage
                        className='narrow winter-neva-gradient '
                        
                        tag='div'
                    >
                        <FcAnswers/>
                    </MDBCardImage>
                    <MDBCardBody cascade className='text-center'>
                        <MDBCardText>
                        Coins Cashout Treshold
                       <MDBBadge className="ml-3" color="light">{profile.CoinsCashoutTreshold}</MDBBadge>
                        </MDBCardText>
                    </MDBCardBody>
                    </MDBCard>
                   </Col>
                  

                   <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage
                        className='narrow winter-neva-gradient '
                        
                        tag='div'
                    >
                        <RiCoinLine/>
                    </MDBCardImage>
                    <MDBCardBody cascade className='text-center'>
                        <MDBCardText>
                       LBP To Coins Ratio
                       <MDBBadge className="ml-3" color="light">{profile.LbpCoinRatio}</MDBBadge>
                        </MDBCardText>
                    </MDBCardBody>
                    </MDBCard>
                   </Col>
                   
                </MDBRow>

                <Col  lg="5" className="m-3 ">
                    
                    
                    <MDBCard  >
                         <MDBCardImage
                             className='narrow-table young-passion-gradient'
                             tag='div'
                         >
                             <h2 className='h2-responsive'>My Profile </h2>
                            

                         </MDBCardImage>
                         <MDBCardBody >
                         { !loading ? 
                            <div>
                             <p>ID:{profile.Id}</p>
                             <p>Municipality Name:{profile.MunicipalityName}</p> 
                             <p>Mohafaza Name: {profile.MohafazaName}</p>
                             <p>Qaza Name :{profile.QazaName}</p>
                            </div>
                         : " "}
                            
                                
                    
                         </MDBCardBody>
                         </MDBCard>

                    
 
                       </Col>
                 
              
             
             
            
            
    
        </div>
    )
}
