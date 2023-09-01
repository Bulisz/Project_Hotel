using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBackend.UnitTests.Services
{
    [TestClass]
    public class RoomServiceTests
    {
        private RoomService _roomService;
        private Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private Mock<Cloudinary> _cloudinaryMock;

        [TestInitialize]
        public void Setup()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _mapperMock = new Mock<IMapper>();
            _cloudinaryMock = new Mock<Cloudinary>("cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name");

            _roomService = new RoomService
                (
                    _roomRepositoryMock.Object,
                    _mapperMock.Object,
                    _cloudinaryMock.Object
                );
                    
                );
        }
    }
}
