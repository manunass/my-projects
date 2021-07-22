import React,{useEffect, useState} from 'react'
import {MDBCard, MDBCardBody, MDBCardImage,MDBBtn ,MDBModal, MDBModalBody, MDBModalHeader,MDBModalFooter} from
'mdbreact';
import {baseUrl} from '../../package.json'


import { MDBDataTable } from 'mdbreact';
import axios from 'axios';
import BussInfo from './BussInfo';

export default function Business() 
{
  const [modal, setmodal] = useState(false)
  const [id, setid] = useState('')
  let content;
  const [business, setbusiness] = useState(['']);
  const [loading, setloading] = useState(true);
  const url=baseUrl+'/api/Businesses?municipalityId=0713b854-89ef-497b-a61c-e0bdb669fa70'
  useEffect(
    () => {
      
      axios.get(url).then(
            res => {
              setbusiness(res.data.Businesses);
              console.log(business);
              setloading(false);
                  }
     )  

},[url]);
const toggle =() =>{
  setmodal(!modal)
}

 const handleClick = (d) =>{
          setmodal(true)    
          setid(d)
                     
  console.log(d)
}

     if(!loading)
     {
     
            content=( business.map(
                  d =>(
                    {
                      id:d.Id,
                      name:d.Name,
                      owner:d.OwnerFirstName +' '+d.OwnerLastName,
                      category:d.Category,
                      phone:d.PhoneNumber,
                      clickEvent: () => {
                        handleClick(d.Id)
                      } 
                    } 
                      )
                )
            ) 
     }
      
     
   
   const  dataList = {
        columns: [
          {
            label:'ID',
            field:'id',
            sort :'asc',
            width:100,
           
          },
          
           {
            label: 'Name',
            field: 'name',
            sort: 'asc',
            width: 150
          },
          {
            label: 'Owner Name',
            field: 'owner',
            sort: 'asc',
            width: 150
          },
          {
            label: 'Category',
            field: 'category',
            sort: 'asc',
            width: 270
          },
          {
            label: 'Phone Number',
            field: 'phone',
            sort: 'asc',
            width: 200
          }
          
          
        ],
       
      
        rows: content
      };
      
    

    return (
      
        <div>
         
        <MDBCard>
          <MDBCardImage
            className='narrow-table primary-color white-text' tag='div' >
            <h2 className='h2-responsive'>Business List </h2>
          </MDBCardImage>
          <MDBCardBody >
           {!loading ?  <MDBDataTable small   data={dataList}/> :<div className="spinner-grow text-success" role="status">
      <span className="sr-only">Loading...</span>
    </div>} 
          </MDBCardBody>
        </MDBCard>

          {modal && <MDBModal isOpen={modal} toggle={toggle}>
                             <MDBModalHeader toggle={toggle}>{id}</MDBModalHeader>
                               <MDBModalBody>
                              <BussInfo ID={id}/>
                               </MDBModalBody>
                               <MDBModalFooter>
                                 <MDBBtn color="primary" onClick={toggle}>Close</MDBBtn>
                                 </MDBModalFooter>
                        </MDBModal>}
        </div>

      
        
    
    )
   
}
