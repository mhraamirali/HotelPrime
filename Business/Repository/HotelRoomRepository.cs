using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext appDbContext;
        private readonly IMapper _mapper;
        public HotelRoomRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            appDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var addHotelRoom = await appDbContext.HotelRooms.AddAsync(hotelRoom);
            await appDbContext.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDTO>(addHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var room = await appDbContext.HotelRooms.FindAsync(roomId);
            if(room!=null)
            {
                appDbContext.HotelRooms.Remove(room);
                return await appDbContext.SaveChangesAsync();
            }
            return 0;
            
        }

        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms()
        {
            try
            {
                IEnumerable<HotelRoomDTO> hotelRoomDTOs =
                    _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>(appDbContext.HotelRooms);
                return hotelRoomDTOs;
                    
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> GetHotelRoom(int roomId)
        {
            try
            {
                HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                    await appDbContext.HotelRooms.FirstOrDefaultAsync(x => x.Id == roomId));
                return hotelRoom;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> IsHotelRoomExist(string name)
        {
            try
            {
                HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                    await appDbContext.HotelRooms.FirstOrDefaultAsync(x => x.Name == name));
                return hotelRoom;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if(roomId==hotelRoomDTO.Id)
                {
                    HotelRoom hotelRoomDetails = await appDbContext.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, hotelRoomDetails);
                    room.CreatedBy = "";
                    room.CreatedDate = DateTime.Now;
                    var updatedRoom = appDbContext.HotelRooms.Update(room);
                    await appDbContext.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
            
        }
    }
}
