﻿using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models;
using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Abstractions.Repositories;

namespace Hotel.Backend.WebAPI.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PostService(IMapper mapper, IPostRepository postRepository, IDateTimeProvider dateTimeProvider)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<PostDetailsDTO> CreatePostAsync(PostCreateDTO postDto)
    {
        Post postToCreate = _mapper.Map<Post>(postDto);
        postToCreate.CreatedAt = _dateTimeProvider.Now;
        return _mapper.Map<PostDetailsDTO>(await _postRepository.CreatePostAsync(postToCreate));
    }

    public async Task<IEnumerable<PostDetailsDTO>> GetAllPostsAsync()
    {
        return _mapper.Map<IEnumerable<PostDetailsDTO>>(await _postRepository.GetAllPostsAsync());
    }
}
